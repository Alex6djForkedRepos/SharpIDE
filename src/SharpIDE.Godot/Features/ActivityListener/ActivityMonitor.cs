using System.Diagnostics;
using SharpIDE.Application;
using SharpIDE.Application.Features.Events;

namespace SharpIDE.Godot.Features.ActivityListener;

public class ActivityMonitor
{
    public EventWrapper<Activity, Task> ActivityStarted { get; } = new(_ => Task.CompletedTask);
    public EventWrapper<Activity, Task> ActivityStopped { get; } = new(_ => Task.CompletedTask);

    public ActivityMonitor()
    {
        var listener = new System.Diagnostics.ActivityListener
        {
            ShouldListenTo = source => source == SharpIdeOtel.Source,
            Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.PropagationData,
            ActivityStarted = activity => ActivityStarted.InvokeParallelFireAndForget(activity),
            ActivityStopped  = activity => ActivityStopped.InvokeParallelFireAndForget(activity),
        };

        ActivitySource.AddActivityListener(listener);
    }
}
using System;
using System.Collections.ObjectModel;
using Avalonia.Animation;
using Avalonia.Rendering.Composition.Animations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TimelineDemo.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    
}

public abstract partial class CompositionAnimationViewModel : ViewModelBase
{
    [ObservableProperty] private string? _target;
}

public abstract partial class KeyFrameAnimationViewModel<T> : ViewModelBase
{
    [ObservableProperty] private AnimationDelayBehavior _delayBehavior;
    [ObservableProperty] private TimeSpan _delayTime;
    [ObservableProperty] private PlaybackDirection _direction;
    [ObservableProperty] private TimeSpan _duration;
    [ObservableProperty] private AnimationIterationBehavior _iterationBehavior;
    [ObservableProperty] private int _iterationCount = 1;
    [ObservableProperty] private AnimationStopBehavior _stopBehavior;
    [ObservableProperty] private ObservableCollection<KeyFrameViewModel<T>>? _keyFrames;
}

public abstract partial class KeyFrameViewModel<T> : ViewModelBase
{
    [ObservableProperty] private float _normalizedProgressKey;
    [ObservableProperty] private T? _value;
    //[ObservableProperty] private Expression Expression;
    //[ObservableProperty] private EasingViewModel EasingFunction;   
}

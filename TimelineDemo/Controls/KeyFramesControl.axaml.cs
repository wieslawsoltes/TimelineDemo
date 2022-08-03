using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace TimelineDemo.Controls;

public class KeyFramesControl : TemplatedControl
{
    public static readonly StyledProperty<double?> ValueProperty =
        AvaloniaProperty.Register<KeyFramesControl, double?>(nameof(Value));

    public static readonly StyledProperty<double?> RangeProperty =
        AvaloniaProperty.Register<KeyFramesControl, double?>(nameof(Range), 100.0);

    private Canvas? _canvas;
    private Thumb? _thumb;
    private bool _updating;
    private bool _captured;

    public double? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double? Range
    {
        get => GetValue(RangeProperty);
        set => SetValue(RangeProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        
        if (_canvas != null)
        {
            _canvas.PointerPressed -= Canvas_PointerPressed;
            _canvas.PointerReleased -= Canvas_PointerReleased;
            _canvas.PointerMoved -= Canvas_PointerMoved;
        }

        if (_thumb != null)
        {
            _thumb.DragDelta -= Thumb_DragDelta;
        }

        _canvas = e.NameScope.Find<Canvas>("PART_Canvas");
        _thumb = e.NameScope.Find<Thumb>("PART_Thumb");

        if (_canvas != null)
        {
            _canvas.PointerPressed += Canvas_PointerPressed;
            _canvas.PointerReleased += Canvas_PointerReleased;
            _canvas.PointerMoved += Canvas_PointerMoved;
        }

        if (_thumb != null)
        {
            _thumb.DragDelta += Thumb_DragDelta;
        }
    }

    private void MoveThumb(Canvas? canvas, Thumb? thumb, double x, double y)
    {
        if (canvas != null && thumb != null)
        {
            var left = Math.Clamp(x, 0, canvas.Bounds.Width);
            var top = Math.Clamp(y, 0, canvas.Bounds.Height);
            Canvas.SetLeft(thumb, left);
            Canvas.SetTop(thumb, top); 
        }
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_canvas is { } && _thumb is { })
        {
            var position = e.GetPosition(_canvas);
            _updating = true;
            MoveThumb(_canvas, _thumb, position.X, 0);
            UpdateValueFromThumb();
            if (_canvas is { } && _thumb is { })
            {
                _canvas.Cursor = new Cursor(StandardCursorType.SizeWestEast);
                _thumb.Cursor = new Cursor(StandardCursorType.SizeWestEast);
            }

            _updating = false;
            e.Pointer.Capture(_canvas);
            _captured = true;
        }
    }

    private void Canvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_captured)
        {
            if (_canvas is { } && _thumb is { })
            {
                _canvas.Cursor = Cursor.Default;
                _thumb.Cursor = Cursor.Default;
            }
            e.Pointer.Capture(null);
            _captured = false;
        }
    }

    private void Canvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (_captured)
        {
            if (_canvas is { } && _thumb is { })
            {
                var position = e.GetPosition(_canvas);
                _updating = true;
                MoveThumb(_canvas, _thumb, position.X, 0);
                UpdateValueFromThumb();
                _updating = false;
            }
        }
    }

    private void Thumb_DragDelta(object? sender, VectorEventArgs e)
    {
        if (_canvas is { } && _thumb is { })
        {
            var position = Canvas.GetLeft(_thumb);
            _updating = true;
            MoveThumb(_canvas, _thumb, position + e.Vector.X, 0);
            UpdateValueFromThumb();
            _updating = false;
        }
    }

    private double GetTrackSize() => _canvas?.Bounds.Width ?? 0.0;

    private void UpdateThumbFromValue()
    {
        if (_canvas is { } && _thumb is { })
        {
            var range = Range ?? 100.0;
            var alphaX = Value * GetTrackSize() / range;
            MoveThumb(_canvas, _thumb, alphaX ?? 0.0, 0.0);
        }
    }

    private void UpdateValueFromThumb()
    {
        if (_thumb is { })
        {
            var range = Range ?? 100.0;
            var position = Canvas.GetLeft(_thumb);
            var value = position * range / GetTrackSize();
            Value = value;
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ValueProperty || change.Property == BoundsProperty)
        {
            if (_updating == false && _canvas is { } && _thumb is { })
            {
                _updating = true;
                UpdateThumbFromValue();
                _updating = false;
            }
        }
    }
}

using Microsoft.JSInterop;

namespace FilmReferenceUI.Shared.Components;

public partial class CKEditorComponent
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    [Parameter] public string? Value { get; set; }

    [Parameter] public EventCallback<string?> ValueChanged { get; set; }

    private string EditorId = $"editor_{Guid.NewGuid()}";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("FilmReference.initCkEditor",
                EditorId,
                Value,
                DotNetObjectReference.Create(this));
        }
    }

    [JSInvokable]
    public async Task OnEditorChanged(string data)
    {
        await ValueChanged.InvokeAsync(data);
    }
}

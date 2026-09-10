namespace FilmReferenceUI.Shared.BasePageClasses;

public abstract class HomeBasePageClass : BasePageClass
{
    [Inject] protected IPersonHandler PersonHandler { get; set; } = null!;
}

namespace IronSmalltalk
{
    public interface ICodeBlock
    {
        SmallObject Execute(ICodeContext context, params SmallObject[] parameters);
    }
}
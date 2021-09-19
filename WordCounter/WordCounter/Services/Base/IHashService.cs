namespace WordCounter.Services.Base
{
    public interface IHashService
    {
        #region Public Members

        string GetHash(string contentString);

        #endregion
    }
}
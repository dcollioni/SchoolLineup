namespace SchoolLineup.Domain.Resources
{
    public class ResourceHelper
    {
        /// <summary>
        /// Esse campo deve ser preenchido.
        /// </summary>
        public static string RequiredField()
        {
            return Messages.RequiredField;
        }

        /// <summary>
        /// Esse campo deve conter, no máximo, {<paramref name="maxLength"/>} caracteres.
        /// </summary>
        public static string MaxLengthField(int maxLength)
        {
            return string.Format(Messages.MaxLengthField, maxLength);
        }
    }
}
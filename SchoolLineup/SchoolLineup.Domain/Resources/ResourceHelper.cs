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

        /// <summary>
        /// Esse campo deve ter um valor único entre os demais registros.
        /// </summary>
        public static string UniqueField()
        {
            return Messages.UniqueField;
        }

        /// <summary>
        /// E-mail inválido.
        /// </summary>
        public static string InvalidEmail()
        {
            return Messages.InvalidEmail;
        }

        /// <summary>
        /// Esse campo deve ser maior do que {0}.
        /// </summary>
        public static string MustBeGreaterThan(string field)
        {
            return string.Format(Messages.MustBeGreaterThan, field);
        }

        /// <summary>
        /// Esse campo deve ser menor do que {0}.
        /// </summary>
        public static string MustBeLessThan(string field)
        {
            return string.Format(Messages.MustBeLessThan, field);
        }

        /// <summary>
        /// Esse registro possui associações e não pode ser excluído.
        /// </summary>
        public static string HasChildren()
        {
            return string.Format(Messages.HasChildren);
        }
    }
}
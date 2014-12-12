namespace SchoolLineup.Web.Mvc.ModelBinding
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Web.Mvc;

    public class U413ModelBinder : DefaultModelBinder
    {
        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            var propertyType = propertyDescriptor.PropertyType;

            if (propertyType == typeof(double))
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).RawValue;
                return double.Parse(value.ToString(), new CultureInfo("en-US"));
            }

            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }
    }
}
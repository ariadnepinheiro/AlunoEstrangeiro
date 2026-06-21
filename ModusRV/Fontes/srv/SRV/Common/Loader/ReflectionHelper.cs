using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text.RegularExpressions;
using SRV.Common.Exceptions;
using System.Globalization;

namespace SRV.Common.Loader
{
    public class ReflectionHelper
    {

        /// <summary>
        /// Gets the import file attribute settings that have been marked for a class
        /// </summary>
        /// <param name="entity">The object whose attribute will be returned</param>
        /// <returns></returns>
        public static ImportFileAttribute GetImportFileAttribute(object entity)
        {
            object[] attributes = entity.GetType().GetCustomAttributes(false);
            foreach (object attribute in attributes)
            {
                if (attribute is ImportFileAttribute)
                {
                    return (ImportFileAttribute)attribute;
                }
            }
            return null;
        }

        public static void SetPropertyValue(object entity, object value, int fieldIndex)
        {
            object[] attributes;

            // Search the properties for the correct position and fill the appropriate value
            foreach (PropertyInfo property in entity.GetType().GetProperties())
            {
                attributes = property.GetCustomAttributes(typeof(ImportFieldAttribute), false);

                foreach (object attribute in attributes)
                {
                    ImportFieldAttribute field = (ImportFieldAttribute)attribute;
                    if (field.Position == fieldIndex)
                    {
                        if (field.Required)
                        {
                            if (((string)value).Trim().Length == 0)
                            {
                                throw new FieldValidationException(
                                    string.Format("Valor é obrigatório para o campo '{0}'", property.Name)
                                    );
                            }
                        }

                        if (field.EnableTrimming)
                        {
                            value = ((string)value).Trim();
                        }

                        if (!field.AllowsPoint)
                        {
                            if (((string)value).IndexOf('.') != -1)
                            {
                                throw new FieldValidationException(
                                    string.Format("Valor inválido para o campo '{0}'", property.Name)
                                    );
                            }
                        }

                        if (IsFieldValueValid(field, value))
                        {
                            try
                            {
                                value = PrepareFieldValue(field, property, value);
                                property.SetValue(entity, value, null);
                            }
                            catch (Exception)
                            {
                                throw new FieldValidationException(
                                    string.Format("Valor inválido para o campo '{0}'", property.Name)
                                    );
                            }
                        }
                        else
                        {
                            throw new FieldValidationException(
                                string.Format("Valor inválido para o campo '{0}'", property.Name)
                                );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines if a field value to be populated to a field is valid or not
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFieldValueValid(ImportFieldAttribute field, object value)
        {
            if (field.EnableValidation)
            {
                if (field.ValidationPattern != null && field.ValidationPattern.Length > 0)
                {
                    if (!Regex.IsMatch((string)value, field.ValidationPattern))
                    {
                        return false;
                    }
                }
                if (field.ValidationLength > 0 && ((string)value).Length != field.ValidationLength)
                {
                    return false;
                }
                if (field.ValidationMaxLength > 0 && ((string)value).Length > field.ValidationMaxLength)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary> 
        /// Sets up the value object for setting to the property
        /// </summary> 
        /// <param name="field"></param> 
        /// <param name="value"></param> 
        /// <returns></returns> 
        public static object PrepareFieldValue(ImportFieldAttribute field, PropertyInfo property, object value)
        {
            // Try to convert the input string value to the proper type 
            // of the data, only if data type is not string 
            if (value is IConvertible && field.DataType != DataType.String)
            {
                Type t = property.PropertyType;

                t = Nullable.GetUnderlyingType(t) ?? t;

                // Adds trailing zeros
                // if the type is decimal
                if (t.Name.Equals("Decimal"))
                {
                    if (!String.IsNullOrEmpty(value.ToString()))
                        value = String.Format("{0:F" + (field.NumDecimalPlaces != 0 ? field.NumDecimalPlaces : Constants.numCasasDecimaisDefault) + "}", value);
                }
                
                value = String.IsNullOrEmpty(value.ToString()) ? null : Convert.ChangeType(value, t, new CultureInfo("pt-BR"));
                
            }
            else
            {
                // Custom conversion types
            }

            return value;
        }

    }
}
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MobileAPIS4.Helpers;

public class TaskAssignModelBinder:IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        if (!bindingContext.ModelMetadata.IsEnumerableType)
        {
            bindingContext.Result=ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        string name=bindingContext.ModelName;
        var valueResult = bindingContext.ValueProvider.GetValue(name);

        if (valueResult == ValueProviderResult.None)
            return Task.CompletedTask;

        var modelValueString = valueResult.FirstValue;
        if (String.IsNullOrEmpty(modelValueString))
        {
            bindingContext.Result=ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        var modelType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
        var converter = TypeDescriptor.GetConverter(modelType);

        var modelValues = modelValueString
            .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(r => converter.ConvertFromString(r.Trim())).ToArray();

        var modelArray = Array.CreateInstance(modelType, modelValues.Length);
        modelValues.CopyTo(modelArray, 0);

        bindingContext.Model = modelArray;
        bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

        return Task.CompletedTask;
    }
}
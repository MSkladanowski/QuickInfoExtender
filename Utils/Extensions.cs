using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickInfoUtils
{
    public static class Extensions
    {
        public static string ToClassificationTypeName(this SymbolDisplayPartKind kind)
        {
            switch (kind)
            {
                case SymbolDisplayPartKind.ErrorTypeName:
                case SymbolDisplayPartKind.RangeVariableName:
                case SymbolDisplayPartKind.AssemblyName:
                case SymbolDisplayPartKind.AliasName:
                    return ClassificationTypeNames.Identifier;
                case SymbolDisplayPartKind.ClassName:
                    return ClassificationTypeNames.ClassName;
                case SymbolDisplayPartKind.DelegateName:
                    return ClassificationTypeNames.DelegateName;
                case SymbolDisplayPartKind.EnumName:
                    return ClassificationTypeNames.EnumName;
                case SymbolDisplayPartKind.EventName:
                    return ClassificationTypeNames.EventName;
                case SymbolDisplayPartKind.FieldName:
                    return ClassificationTypeNames.FieldName;
                case SymbolDisplayPartKind.InterfaceName:
                    return ClassificationTypeNames.InterfaceName;
                case SymbolDisplayPartKind.Keyword:
                    return ClassificationTypeNames.Keyword;
                case SymbolDisplayPartKind.LabelName:
                    return ClassificationTypeNames.LabelName;
                case SymbolDisplayPartKind.LineBreak:
                case SymbolDisplayPartKind.Space:
                    return ClassificationTypeNames.WhiteSpace;
                case SymbolDisplayPartKind.NumericLiteral:
                    return ClassificationTypeNames.NumericLiteral;
                case SymbolDisplayPartKind.StringLiteral:
                    return ClassificationTypeNames.StringLiteral;
                case SymbolDisplayPartKind.LocalName:
                    return ClassificationTypeNames.LocalName;
                case SymbolDisplayPartKind.MethodName:
                    return ClassificationTypeNames.MethodName;
                case SymbolDisplayPartKind.ModuleName:
                    return ClassificationTypeNames.ModuleName;
                case SymbolDisplayPartKind.NamespaceName:
                    return ClassificationTypeNames.NamespaceName;
                case SymbolDisplayPartKind.Operator:
                    return ClassificationTypeNames.Operator;
                case SymbolDisplayPartKind.ParameterName:
                    return ClassificationTypeNames.ParameterName;
                case SymbolDisplayPartKind.PropertyName:
                    return ClassificationTypeNames.PropertyName;
                case SymbolDisplayPartKind.Punctuation:
                    return ClassificationTypeNames.Punctuation;
                case SymbolDisplayPartKind.StructName:
                    return ClassificationTypeNames.StructName;
                case SymbolDisplayPartKind.AnonymousTypeIndicator:
                case SymbolDisplayPartKind.Text:
                    return ClassificationTypeNames.Text;
                case SymbolDisplayPartKind.TypeParameterName:
                    return ClassificationTypeNames.TypeParameterName;

                default:
                    return string.Empty;
            }
        }
    }
}

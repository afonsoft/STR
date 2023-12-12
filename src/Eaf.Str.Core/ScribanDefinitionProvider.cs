using Eaf.TextTemplating;
using Eaf.TextTemplating.Scriban;

namespace Eaf.Str
{
    /// <summary>
    /// Scriban Definition Provider for template tpl
    /// </summary>
    public class ScribanDefinitionProvider : TemplateDefinitionProvider
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            // context.Add(new TemplateDefinition(
            //     "NameTemplate",
            //     defaultCultureName: "pt")
            //     .WithVirtualFilePath("path/template.tpl", true)
            //     .WithScribanEngine()
            //);
        }
    }
}
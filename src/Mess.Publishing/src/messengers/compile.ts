import Handlebars from "handlebars";

const compile = (template: string, data: Record<string, unknown>) => {
  let compiled = compilationCache.get(template);
  if (!compiled) {
    compiled = Handlebars.compile(template);
    compilationCache.set(template, compiled);
  }

  return compiled(data);
};

const compilationCache = new Map<string, Handlebars.TemplateDelegate>();

export default compile;

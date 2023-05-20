import Handlebars from "handlebars";

export type Create = (
  template: string,
) => PromiseLike<string | string[]> | string | string[];

export const compile = (template: string, data: Record<string, unknown>) => {
  let compiled = compilationCache.get(template);
  if (!compiled) {
    compiled = Handlebars.compile(template);
    compilationCache.set(template, compiled);
  }

  return compiled(data);
};

const compilationCache = new Map<string, Handlebars.TemplateDelegate>();

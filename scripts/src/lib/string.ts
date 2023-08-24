export const isNullOrWhitespace = (value?: string | null | undefined) => {
  return !value || value.trim().length === 0;
};

export const hasValue = (value?: string | null | undefined) => {
  return !isNullOrWhitespace(value);
};

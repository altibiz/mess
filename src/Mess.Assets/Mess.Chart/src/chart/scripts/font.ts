import { FontSpec } from "chart.js";
import FontFaceObserver from "fontfaceobserver";

export type Font = FontSpec & {
  color: string;
};

export const loadFont = async ({ family, style = "normal", weight }: Font) => {
  const fontObserver = new FontFaceObserver(family, {
    style,
    weight: weight ?? undefined,
  });

  await fontObserver.load();
};

export default loadFont;

import {
  EorIotDeviceData,
  eorIotDeviceDataSchema,
} from "./schema";

export const getData = async (
  requestUrlPrefix: string,
  contentItemId: string,
): Promise<EorIotDeviceData> => {
  const response = await fetch(
    `${requestUrlPrefix}/Devices/${contentItemId}/Data`,
  );
  const rawData = await response.json();
  const parsedData = await eorIotDeviceDataSchema.parseAsync(rawData);
  return parsedData;
};

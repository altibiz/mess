import {
  EorMeasurementDeviceData,
  eorMeasurementDeviceDataSchema,
} from "./schema";

export const getData = async (
  requestUrlPrefix: string,
  contentItemId: string,
): Promise<EorMeasurementDeviceData> => {
  const response = await fetch(
    `${requestUrlPrefix}/Devices/${contentItemId}/Data`,
  );
  const rawData = await response.json();
  const parsedData = await eorMeasurementDeviceDataSchema.parseAsync(rawData);
  return parsedData;
};

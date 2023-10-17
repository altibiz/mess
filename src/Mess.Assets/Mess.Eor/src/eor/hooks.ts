import { getData } from "./api";

// TODO: translation somehow

export type UseRefreshProps = {
  requestUrlPrefix: string;
  contentItemId: string;
  id: {
    status: string;
    communicationFault: string;
    processFault: string;
    processFaults: string;
    controlStatus: string;
    modeLabel: string;
    modeInput: string;
    doorState: string;
    mainCircuitBreakerState: string;
    powerRelay: string;
    voltage: string;
    current: string;
    temperature: string;
  };
};

export const useRefresh = ({
  requestUrlPrefix,
  contentItemId,
  id,
}: UseRefreshProps) => {
  let interval: number | null = null;
  window.addEventListener("load", () => {
    interval = window.setInterval(handleSecondPassed, 1000);
  });
  window.addEventListener("unload", () => {
    if (interval) {
      clearInterval(interval);
    }
  });

  const handleSecondPassed = async () => {
    const data = await getData(requestUrlPrefix, contentItemId);

    const status =
      data.eorIotDeviceSummary.status.processFault === 0
        ? { text: "OK", class: "status status-ok" }
        : data.eorIotDeviceSummary.status.processFault === 999
        ? { text: "OFF", class: "status status-warning" }
        : { text: "ERROR", class: "status status-error" };
    setElementText(id.status, status.text);
    setElementClass(id.status, status.class);

    setElementText(
      id.communicationFault,
      data.eorIotDeviceSummary.status.communicationFault,
    );

    setElementText(
      id.processFault,
      data.eorIotDeviceSummary.status.processFault,
    );
    setElementInnerHtml(
      id.processFaults,
      data.eorIotDeviceSummary.status.processFaults
        ? "<p>" +
            data.eorIotDeviceSummary.status.processFaults.join("<p>")
        : "",
    );

    const deviceControlStatus =
      data.eorIotDeviceSummary.status.resetState === "ShouldReset"
        ? { text: "Reset", class: "status status-warning" }
        : data.eorIotDeviceSummary.status.runState === "Started"
        ? { text: "Started", class: "status status-ok" }
        : data.eorIotDeviceSummary.status.runState === "Stopped"
        ? { text: "Stopped", class: "status status-error" }
        : { text: "Unknown", class: "status status-warning" };
    const serverControlStatus =
      data.eorIotDeviceControls.resetState === "ShouldReset"
        ? { text: "Reset", class: "status status-warning" }
        : data.eorIotDeviceControls.runState === "Started"
        ? { text: "Started", class: "status status-ok" }
        : data.eorIotDeviceControls.runState === "Stopped"
        ? { text: "Stopped", class: "status status-error" }
        : { text: "Unknown", class: "status status-warning" };
    setElementInnerHtml(
      id.controlStatus,
      deviceControlStatus.text !== serverControlStatus.text
        ? `${serverControlStatus.text} <span>(${deviceControlStatus.text})</span>`
        : serverControlStatus.text,
    );
    setElementClass(id.controlStatus, serverControlStatus.class);

    setElementInnerHtml(
      id.modeLabel,
      data.eorIotDeviceControls.mode !=
        data.eorIotDeviceSummary.status.mode
        ? `Position (${data.eorIotDeviceSummary.status.mode})`
        : `Position`,
    );
    setInputElementValue(id.modeInput, data.eorIotDeviceControls.mode);

    setElementText(
      id.doorState,
      data.eorIotDeviceSummary.status.doorState,
    );

    setElementText(
      id.mainCircuitBreakerState,
      data.eorIotDeviceSummary.status.mainCircuitBreakerState,
    );

    setElementText(
      id.powerRelay,
      data.eorIotDeviceSummary.status.processFault === 0
        ? "On"
        : data.eorIotDeviceSummary.status.processFault === 999
        ? "Off"
        : "Error",
    );

    setElementText(
      id.voltage,
      data.eorIotDeviceSummary.lastMeasurement.voltage,
    );

    setElementText(
      id.current,
      data.eorIotDeviceSummary.lastMeasurement.current,
    );

    setElementText(
      id.temperature,
      data.eorIotDeviceSummary.lastMeasurement.temperature,
    );
  };
};

const setElementClass = (id: string, className: { toString(): string }) => {
  const element = document.getElementById(id);
  if (!element) {
    return;
  }

  element.className = className.toString();
};

const setElementText = (id: string, text: { toString(): string }) => {
  const element = document.getElementById(id);
  if (!element) {
    return;
  }

  element.textContent = text.toString();
};

const setElementInnerHtml = (id: string, html: { toString(): string }) => {
  const element = document.getElementById(id);
  if (!element) {
    return;
  }

  element.innerHTML = html.toString();
};

const setInputElementValue = (id: string, value: { toString(): string }) => {
  const element = document.getElementById(id) as HTMLInputElement | null;
  if (
    !element ||
    element.tagName !== "INPUT" ||
    document.activeElement === element
  ) {
    return;
  }

  element.value = value.toString();
};

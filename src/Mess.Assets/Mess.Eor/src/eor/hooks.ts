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

    // Default status and lastMeasurement objects to prevent undefined values
    const status = data.summary.status || {
      processFault: 0,
      communicationFault: 0,
      processFaults: [],
      resetState: "ShouldntReset",
      runState: "Stopped",
      doorState: "Closed",
      mainCircuitBreakerState: "Off",
      mode: 0,
    };

    const lastMeasurement = data.summary.lastMeasurement || {
      voltage: 0,
      current: 0,
      temperature: 0,
    };

    const statusDisplay =
      status.processFault === 0
        ? { text: "OK", class: "status status-ok" }
        : status.processFault === 999
          ? { text: "OFF", class: "status status-warning" }
          : { text: "ERROR", class: "status status-error" };
    setElementText(id.status, statusDisplay.text);
    setElementClass(id.status, statusDisplay.class);

    setElementText(id.communicationFault, status.communicationFault);
    setElementText(id.processFault, status.processFault);
    setElementInnerHtml(
      id.processFaults,
      status.processFaults && status.processFaults.length > 0
        ? "<p>" + status.processFaults.join("<p>")
        : "<p>No Faults</p>",
    );

    const deviceControlStatus =
      status.resetState === "ShouldReset"
        ? { text: "Reset", class: "status status-warning" }
        : status.runState === "Started"
          ? { text: "Started", class: "status status-ok" }
          : status.runState === "Stopped"
            ? { text: "Stopped", class: "status status-error" }
            : { text: "Unknown", class: "status status-warning" };

    const serverControlStatus = {
      text: "Unknown",
      class: "status status-warning",
    };

    setElementInnerHtml(
      id.controlStatus,
      deviceControlStatus.text !== serverControlStatus.text
        ? `${serverControlStatus.text} <span>(${deviceControlStatus.text})</span>`
        : serverControlStatus.text,
    );
    setElementClass(id.controlStatus, deviceControlStatus.class);

    setElementInnerHtml(
      id.modeLabel,
      data.controls.mode !== status.mode
        ? `Position (${status.mode})`
        : `Position`,
    );
    setInputElementValue(id.modeInput, data.controls.mode);

    setElementText(id.doorState, status.doorState);
    setElementText(id.mainCircuitBreakerState, status.mainCircuitBreakerState);

    setElementText(
      id.powerRelay,
      status.processFault === 0
        ? "On"
        : status.processFault === 999
          ? "Off"
          : "Error",
    );

    setElementText(id.voltage, lastMeasurement.voltage);
    setElementText(id.current, lastMeasurement.current);
    setElementText(id.temperature, lastMeasurement.temperature);
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

  element.textContent = window.translations[text.toString()];
};

const setElementInnerHtml = (id: string, html: { toString(): string }) => {
  const element = document.getElementById(id);
  if (!element) {
    return;
  }

  const originalText = html.toString();
  const words = originalText.split(/([^\w\s]+)/);
  const translatedWords = words.map((word) => {
    return window.translations && window.translations[word.trim()]
      ? window.translations[word.trim()]
      : word;
  });
  const translatedHtml = translatedWords.join("");

  element.innerHTML = translatedHtml;
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

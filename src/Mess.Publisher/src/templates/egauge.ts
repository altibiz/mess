import { Create, compile } from ".";
import { DateTime } from "luxon";

const create: Create = async (template) => {
  const now = DateTime.now().toUTC();
  const data = [
    {
      timestamp: `0x${now.toUnixInteger().toString(16)}`,
      power: (Math.random() * 1000).toFixed(0),
      voltage: (Math.random() * 2 + 239).toFixed(0),
    },
  ];

  return data.map((data) => compile(template, data));
};

export default create;

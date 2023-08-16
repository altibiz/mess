import * as readline from "readline";

export function prompt(question: string): Promise<boolean> {
  const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout,
  });

  return new Promise((resolve) => {
    rl.question(`[mess]: ${question} (y/n) `, (answer) => {
      rl.write("\n\n");
      rl.close();
      resolve(answer.toLowerCase() === "y");
    });
  });
}

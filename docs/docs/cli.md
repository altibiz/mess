---
sidebar_position: 2
---

# CLI

The CLI's entry point are the `mess` scripts located at the root of the
repository. They are a wrapper around the `scripts` project which also check for
core dependencies of Mess. The entry point is a bash script for Linux and a
batch+powershell script for Windows.

The `scripts` project is composed of a collection of utility functions in the
`scripts/src/lib` directory and a collection of commands in the
`scripts/src/cmds` directory. Below is and outline of the commands and their
usage. You can also run `mess --help` to get a list of commands and their usage.

## `mess prepare`

This command prepares the repository for development. It will install all
dependencies needed to start developing.

You can pass the `--skip` argument to skip a particular set of dependencies.
Available options are `test` and `hooks`.

You can also pass the `--build` argument to build the repository after preparing
it. This is useful for development because your IDE will become immediately
useful after opening the repository.

| Option    | Description                              |
| --------- | ---------------------------------------- |
| `--skip`  | Skip a particular set of dependencies.   |
| `--build` | Build the repository after preparing it. |

## `mess dev`

This command starts the development server. It will start docker services,
chosen publishers via [publishing](/mess/docs/publishing) and watch for changes
in various projects and rebuild the project when changes are detected. Note that
you have to reload the page manually to see the changes.

You can also clean the `App_Data` and database before starting the development
to rerun all migrations and get a clean slate.

To debug, you can pass the `--debug` argument to not run dotnet and debug with
another tool like VS Code or Visual Studio.

| Option     | Description                            |
| ---------- | -------------------------------------- |
| `--update` | Run updating publishers.               |
| `--push`   | Run pushing publishers.                |
| `--clean`  | Clean relevant artifacts before watch. |
| `--debug`  | Don't run dotnet to debug.             |

## `mess test`

This command runs the tests. We use `XUnit` as a test runner and `Playwright`
for browser automation. This command takes no arguments, but passes all
arguments after `--` to `dotnet test`.

## `mess migrate`

This command generates Entity Framework migrations for a single module.

| Positional | Description                            |
| ---------- | -------------------------------------- |
| `project`  | The module to generate migrations for. |
| `name`     | The name of the migration.             |

| Option     | Description                                        |
| ---------- | -------------------------------------------------- |
| `--format` | Format the generated migration (defaults to true). |

## `mess plop`

Generate boilerplate code. It uses the `scripts/src/plop` directory in which it
looks for the needed templates.

### `mess plop module <name> <description>`

This command generates a new module.

| Positional    | Description                    |
| ------------- | ------------------------------ |
| `name`        | The name of the module.        |
| `description` | The description of the module. |

| Option     | Description                                            |
| ---------- | ------------------------------------------------------ |
| `--test`   | Plop test projects (defaults to true).                 |
| `--assets` | Also plop an assets package (defaults to false).       |
| `--format` | Format repository after generation (defaults to true). |

### `mess plop theme <name> <description>`

This command generates a new theme.

| Positional    | Description                   |
| ------------- | ----------------------------- |
| `name`        | The name of the theme.        |
| `description` | The description of the theme. |

| Option     | Description                                            |
| ---------- | ------------------------------------------------------ |
| `--assets` | Also plop an assets package (defaults to false).       |
| `--format` | Format repository after generation (defaults to true). |

## `mess format`

Format the Mess repository. It uses `prettier` to format most files and
`csharpier` to format C# files.

## `mess lint`

Lint the Mess repository. This will run on each commit via a git hook if you
installed husky hooks.

It will first run `prettier` and `csharpier` to check if all files are formatted
properly and then `yarn workspaces foreach` to lint all Yarn workspaces and
`dotnet build` to lint C# projects. Yarn workspaces usually use `eslint` to lint
JS and TS and `stylelint` to lint CSS and SCSS.

| Option          | Description                          |
| --------------- | ------------------------------------ |
| `--format-only` | Only run `prettier` and `csharpier`. |

## `mess docs`

Commands to manage the documentation.

### `mess docs dev`

Starts the documentation server.

## `mess clean`

This command cleans the repository. It will clean Orchard Core `App_Data` and
the database. Passing the `--all` argument will also clean all debug artifacts
via `git clean`.

| Option  | Description          |
| ------- | -------------------- |
| `--all` | Clean all artifacts. |

## `mess publish`

This command publishes the repository. It will build the project and publish it
to the `artifacts` directory unless the `--publish-dir` argument is passed.

It also has the option to launch the published artifacts. This is useful to test
the published artifacts when a bug is not reproducible in a development
environment. We would like to avoid this scenario as much as possible.

| Option          | Description                              |
| --------------- | ---------------------------------------- |
| `--publish-dir` | The directory to publish the project to. |
| `--launch`      | Launch the published artifacts.          |

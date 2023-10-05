---
sidebar_position: 1
---

# Mess

Mess uses a monorepo organization to manage the codebase. It is split off into a
multitude of C# and JS projects. Further, certain projects have a specific type.

## CLI

The best way to get started developing Mess is to understand its `scripts`
project. Scripts is a JS project that contains a single CLI to manage all things
Mess such as development, testing, building, linting, formatting, etc.

To simply get started, run `mess prepare` and `mess dev` in the root of the
repository.

You can read up on how to use it in the [CLI](/mess/docs/cli) chapter.

## Server

The server is composed of a collection of C# and JS projects. The `Mess.Web`
project is the entry point. Organization of the Mess server takes heavy
inspiration from how [Orchard Core](https://github.com/OrchardCMS/OrchardCore)
is organized.

### Modules

The `src/Mess.Modules` directory contains modules that are the core
functionality of Mess and the `src/Mess.Abstractions` directory contains
abstractions to those functionalities so that modules that depend on each other
don't depend on each others implementation.

The `test/Mess.Modules` directory contains tests for the modules and the
`test/Mess.Abstractions` contains abstractions for when testing modules depend
on other testing modules in integration testing scenarios.

Learn more about each module in the [Modules](/mess/docs/modules) chapter.

### Themes

The `src/Mess.Themes` directory contains themes that are the core UI of Mess.

Learn more about each theme in the [Themes](/mess/docs/themes) chapter.

### Assets

Some modules and themes also contain assets in the `src/Mess.Assets` folder
which contains static assets such as images, fonts, scripts and styles. These
assets are managed by the `src/Mess.Pack` project that builds webpack
configurations for each module and theme.

`src/Mess.Pack` also contains a couple of other packages that are used to
configure the developer experience of asset packages across all modules and
themes.

Each module and theme that has assets also contains a section about them in
their respective chapters.

## Utilities

There are a couple of utility projects in the `src` directory (`Mess.System` and
`Mess.OrchardCore`) that provide various utility functions to modules. These are
separated in two different projects because `Mess.System` does not depend on
Orchard Core and `Mess.OrchardCore` does. By doing this we can extend Mess
without relying on Orchard Core.

This is reflected in the `test` directory where `Mess.System.Tests` and
`Mess.OrchardCore.Tests` contain tests for the respective projects, but also
provide utilities for module test projects.

Learn more about utilities in the [Utilities](/mess/docs/utilities) chapter.

## Publishing

The `src/Mess.Publishing` project is a JS project that contains a single CLI
that can simulate various publishing scenarios. It is used to fake various
publishing sources to the event store, but also fake the event store when
developing code for various publishers. It is a simulation of the external world
for Mess and also Mess for the external world.

Learn more about publishing in the [Publishing](/mess/docs/publishing) chapter.

## Documentation

Last but not least, the `docs` directory contains the documentation for Mess. It
is a Docusaurus project that is built and deployed to GitHub pages.

Learn more about the documentation in the [Documentation](/mess/docs/docs)
chapter.

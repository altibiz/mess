// @ts-check
// Note: type annotations allow type checking and IDEs autocompletion

const lightCodeTheme = require("prism-react-renderer/themes/github");
const darkCodeTheme = require("prism-react-renderer/themes/dracula");

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: "Mess",
  tagline: "Measurement Event Store (Something)",
  favicon: "img/favicon.ico",

  url: "https://altibiz.github.io",

  baseUrl: "/mess/",

  organizationName: "altibiz",
  projectName: "mess",

  onBrokenLinks: "throw",
  onBrokenMarkdownLinks: "warn",

  i18n: {
    defaultLocale: "en",
    locales: ["en"],
  },

  presets: [
    [
      "classic",
      /** @type {import('@docusaurus/preset-classic').Options} */
      ({
        docs: {
          sidebarPath: require.resolve("./sidebars.js"),
          editUrl: "https://github.com/altibiz/mess",
        },
        blog: {
          showReadingTime: true,
          editUrl: "https://github.com/altibiz/mess",
        },
        theme: {
          customCss: require.resolve("./src/css/custom.css"),
        },
      }),
    ],
  ],

  themeConfig:
    /** @type {import('@docusaurus/preset-classic').ThemeConfig} */
    ({
      image: "images/social-card.jpg",
      navbar: {
        title: "Mess",
        logo: {
          alt: "Mess",
          src: "images/logo.svg",
        },
        items: [
          {
            type: "docSidebar",
            sidebarId: "sidebar",
            position: "left",
            label: "Docs",
          },
          {
            href: "https://github.com/altibiz/mess",
            label: "GitHub",
            position: "right",
          },
        ],
      },
      footer: {
        style: "dark",
        links: [
          {
            title: "Docs",
            items: [
              {
                label: "Introduction",
                to: "/docs/intro",
              },
            ],
          },
          {
            title: "More",
            items: [
              {
                label: "GitHub",
                href: "https://github.com/altibiz/mess",
              },
            ],
          },
        ],
        copyright: `Copyright © ${new Date().getFullYear()} Altibiz`,
      },
      prism: {
        theme: lightCodeTheme,
        darkTheme: darkCodeTheme,
      },
    }),
};

module.exports = config;

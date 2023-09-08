import Link from "@docusaurus/Link";
import useDocusaurusContext from "@docusaurus/useDocusaurusContext";
import Layout from "@theme/Layout";
import clsx from "clsx";
import React from "react";

import styles from "./index.module.css";

export default function Home(): JSX.Element {
  const { siteConfig } = useDocusaurusContext();

  return (
    <Layout
      title={`${siteConfig.title} documentation`}
      description={`${siteConfig.tagline}`}
    >
      <header className={clsx("hero hero--primary", styles.heroBanner)}>
        <div className="container">
          <h1 className="hero__title">{siteConfig.title}</h1>
          <p className="hero__subtitle">{siteConfig.tagline}</p>
          <div className={styles.buttons}>
            <Link
              className="button button--secondary button--lg"
              to="/docs/intro"
            >
              Docs
            </Link>
          </div>
        </div>
      </header>
      <main className={styles.description}>
        <p>
          Mess is a web application designed to accumulate measurements and
          metric data from various sources and display it in an easily
          digestible web interface. It leverages the event store pattern to
          handle data via push and pull mechanisms, providing a central hub for
          all your metric data needs.
        </p>
      </main>
    </Layout>
  );
}

import React, { Component, useEffect } from "react";
import axios from "axios";
import { Route } from "react-router";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { FetchData } from "./components/FetchData";
import { Counter } from "./components/Counter";

import "./custom.css";

export const App = () => {
    useEffect(() => {
        axios("api/WorkSections/GetListOfSections");
    }, []);
    return (
        <Layout>
            <Route exact path="/" component={Home} />
            <Route path="/counter" component={Counter} />
            <Route path="/fetch-data" component={FetchData} />
        </Layout>
    );
};

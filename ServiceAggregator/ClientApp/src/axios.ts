import axios from "axios";

const host = "https://localhost:7280/api";

const appAxios = axios.create({ baseURL: host });

export default appAxios;

appAxios.get("Account/Get");

import http from "k6/http";
import { check, sleep } from "k6";

export let options = {
  stages: [
    { duration: "10s", target: 100 },
    { duration: "10s", target: 600 },
    { duration: "10s", target: 1000 },
    { duration: "10s", target: 10000 },
  ],
};

const BASE_URL = "http://127.0.0.1:55942/api/auth";

const ENDPOINTS = ["/getHealth"];

export default function () {
  for (const endpoint of ENDPOINTS) {
    let res = http.get(BASE_URL + endpoint);

    // Check the status is 200
    check(res, {
      "status was 200": (r) => r.status == 200,
    }); 
  }

  sleep(1);
}
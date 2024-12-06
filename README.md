# Weather Station API

A simple backend-focused RESTful API service to process, cache, and expose weather station data for temperature analysis.

---

## Code architecture

-   **'Data' folder**:
    -   Contains a small sample .txt file, that was (besides the main measurements.txt)  used for testing.
-   **'Postman' folder**:
    -   Contains the postman export files, as Postman was used for testing.

---

## Runtime details

-   In my case, the program took a few seconds to compile and start running.
    -   The GET ```/api/Cities``` request also takes a few seconds to process (though it is faster than the program's initial startup)
    -   Similarly, the POST ```/api/cities/reload``` endpoint takes a few seconds to complete.

---

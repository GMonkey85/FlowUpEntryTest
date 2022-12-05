# FlowUpEntryTest

This repository contains implementation of https://gist.github.com/vmasek/ae519af99cd33210661aa16981ef72c7, hopefully with acceptable behavior.

## Build info
Projects from this repository are targeted to net7.0.

## Implementation notes

* aggregation endpoint response is not exactly according to example, current implementation returns JSON
* **PseudoUnitTests** project contains sample of automated tests
* Bonus task is not finished, but in its current state of implementation is in **EndToEndTests** project in form of automated end to end test
* it was not specified, but for easier human interaction, web service returns some additional texts in response body (POST OK case and for Error cases)

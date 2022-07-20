# ParcelLookup
This project is a prototype designed to replace King County's existing GIS Districts Reports web app.
![localhost_7025__PIN=1924089026](https://user-images.githubusercontent.com/11726956/180100018-3b03c3c5-667f-4723-be1d-d920b7177c64.png)

It faithfully reimplements the same functionally with the primary goal for improving page load times. You can test the performance improvement for yourself by running the app locally and using the example links on the default page to compare the load times for the Existing app and the New app (this project). I recommend using Chrome's inspect tool, going to the Network tab and the using CTRL + F5 to clear the cache and reload the current page. Then you can see the page load time in the graph and stats in the Network tab.

In my testing the reduction in page load time is clear.
Parcel        |Existing|New     |
--------------|--------|--------|
Seattle       |4.21s   |1.48s   |
Sensitive Area|9.13s   |2.78s   |
Tukwila       |9.72s   |2.49s   |
No Address    |7.34s   |2.26s   |
Condo         |3.75s   |1.42s   |

Were this prototype to continue I would like to page load times continue to improve where the best case takes less than one second and the worst case at most 2 seconds.

# HTTP Request and Response workflow
![image](https://user-images.githubusercontent.com/11726956/180094086-0e8d2385-040f-4aa6-9614-b99339cdb593.png)
This diagram show the request/response workflow that this app currently implments. I believe there maybe addional room for improvement by removing the HTTP call to the KC GIS Lookup API and implementing that functionally directly into this app using a database query. The response from the Districts Report service definition request could also be cached, as it doesn't change very often, so that we can skip that HTTP call nearly all of the time.

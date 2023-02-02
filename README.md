# ParcelLookup 🏡
This project is a prototype designed to replace King County's existing GIS Districts Reports web app.

# [TRY IT HERE](https://parcellookup.azurewebsites.net/)

![parcellookup azurewebsites net__PIN=0761000160](https://user-images.githubusercontent.com/11726956/215925903-bc7c4c82-052f-4c1b-ad31-8b9bfb0cddb7.png)

It faithfully reimplements the same functionally with the primary goal of improving page load times. You can test the performance improvement for yourself by running this app locally and using the example links on the default page to compare the load times for the Existing app and the New app (this project). I recommend using Chrome's inspect tool, going to the Network tab, and then hitting CTRL + F5 to clear the cache and reload the current page. You'll see the page load time on the graph and in stats on Network tab.

In my testing the reduction in page load time is clear.
Parcel        |Existing|New     |
--------------|--------|--------|
Seattle       |4.21s   |1.48s   |
Sensitive Area|9.13s   |2.78s   |
Tukwila       |9.72s   |2.49s   |
No Address    |7.34s   |2.26s   |
Condo         |3.75s   |1.42s   |

# Styling Update
This app uses the new King County LIQUID design system and is based on the work that the UX team is doing for the new County website and Sitecore upgrade project.

Here is a side-by-side comparison of the existing app and this one using the same parcel data:

![image](https://user-images.githubusercontent.com/11726956/215926155-22514b9f-f8d2-465e-b0ba-a1852a99687f.png)

# HTTP Request and Response workflow 🙋‍♂️
This diagram shows the request/response workflow that this app currently implements:

![image](https://user-images.githubusercontent.com/11726956/180094086-0e8d2385-040f-4aa6-9614-b99339cdb593.png)

I believe there maybe additional room for improvement by removing the HTTP call to the KC GIS Lookup API and implementing that functionally directly into this app using a database query. The response from the Districts Report service definition request could also be cached, as it doesn't change very often, so that we can skip that HTTP call nearly all the time.

# How to contribute ✏
If you find a bug or want a feature, please file an issue with the steps to reproduce the bug or a description of the functionally you need.

# How to run 👟
- Install Visual Studio 2022 on your machine.
- Clone this repo to your machine.
- Open the root folder of this repo.
- Launch the project into Visual Studio by double-clicking the "ParcelLookup.sln" file (or opening it from Visual Studio using the file explorer pane).
- Run the project in debug mode by hitting the F5 key or clicking on the green arrow near the center of the top navbar in Visual Studio. 
- Have fun reading the District Reports for the example parcels on the default page. 🚀


This is a .NET library for Slik.IO - Charts as a service.

#Installation
###Nuget 
`Install-Package SlikIO`

#Usage
First, sign up to Slik.IO if you havent done so already and get the PRIVATE API KEY.
Initialize the framework:
```ruby
SlikIO slikio = new SlikIO("YOUR_PRIVATE_API_KEY");
```

###Pushing data to collections
You can push data to collections using the `sendData` method.
```ruby
slikio.sendData("COLLECTION_ID", Dictionary<string,object>);
```
Example:
```ruby
slikio.sendData("col_3b057f15e4", new Dictionary<string,object>() {
                {"userId", "123123"},
                {"email", "user@email.com"},
                {"action", "planPurchased"},
                {"cost", 150.0}
            });
```
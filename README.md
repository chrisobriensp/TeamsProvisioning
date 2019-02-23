# COB resources for provisioning Microsoft Teams from a template

Please see accompanying blog post - <a href="https://cob-sp.com/TeamsProvisioning">https://cob-sp.com/TeamsProvisioning</a>

## This project includes:

- A static JSON file which provides the non-changing elements of a Teams template
- Code to manipulate the JSON to add bits that change for each instance of a Team - the name, description, and owner for example. This is in the form of a C# console app
- Code to make the appropriate call to the Microsoft Graph to provision the Team from the JSON

## The setup

You'll need an AAD app registration and to swap out placeholders in the code for your client ID/secret and tenant details. Of course, in real life you'll need to deal with these appropriately for production code.

## The result

The code will create as many Teams as you like, all containining the channels, tabs and apps defined in the template:

![Team created from template - 1](https://3.bp.blogspot.com/-SWHd_Sn42xs/XHHCOvTB1KI/AAAAAAAAGB0/AtaJUeq3yEI1TOqhGPujtHXGoO_o7YVxACLcBGAs/s1600/CreatedTeam1.png)

![Team created from template - 2](https://3.bp.blogspot.com/-cl75zNLL3zM/XHHCOtE95SI/AAAAAAAAGBw/8hbqfcdez3oXj7Uaifl3I1WPcEnKfRW8QCLcBGAs/s1600/CreatedTeam2.png)

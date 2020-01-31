# BitwardenToChromePasswordConverter
 A simple console app that converts BitWarden .csv password files to Chrome/Chromium .csv files for password import.

<p align="center">
  <img src="./images/1.png?raw=true" width="100%" title="BitwardenToChromePasswordConverter">
</p>

## How to use:
###### See [Releases](https://github.com/petar-staynov/BitwardenToChromePasswordConverter/releases "Releases") for download. Otherwise compile as a .NET Core app.
#### Export your BitWarden passwords to a .csv file
- Go to BitWarden -> Settings -> Export Vault
- Select ".csv" as file format
- Type your Master password and click Submit
- The browser will download a file containing your password

#### Enable password importing in Chrome/Chromium METHOD 1 (Windows)
- Create a Chrome shortcut on your desktop.
- Right-Click and go to properties.
- In the target section just add the parameter –enable-features=PasswordImport to the end of the line.
- Start the browser from the newly created shortcut
- Enter chrome://settings/passwords in the address bar
- Click on the 3-dots dropdown to show the Import passwords option
- Select the passwords file that you created using my program.

#### Enable password importing in Chrome/Chromium METHOD 2
- Enter chrome://settings/passwords in the address bar
- Click on the 3-dots dropdown to show the "Export passwords..." option
- Right click the "Export passwords..." option and select the "Inspect" option
- You will see the line of code for the EXPORT option, called "menuExportPassword"
- Above it you will see the line of code for the IMPORT option, called "menuImportPassword"
- In the Import code locate the word "hidden", select it (double click) and delete it
- Click the ENTER key, and you’re all done! (you can close the inspect console)
- Now, when you will click the 3-dots dropdown you will see the "Import" option as well
- Select the passwords file that you created using my program.

<p align="center">
  <img src="./images/2.png?raw=true" width="100%" title="Chrome Settings">
</p>

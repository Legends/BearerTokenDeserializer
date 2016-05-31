# BearerTokenDeserializer
Deserialize BearerTokens (Windows Forms tool)

This tool decrypts BearerTokens using the MachineKeyDataProtector for IIS hosted applications.
You have to modify the app.config to enter your encryption and validation key, validation algorythm:

`<machineKey decryptionKey="B7EF...82F408D2ECBFAC817" 
validation="SHA1" 
validationKey="C2B8D...8865D68599BF89EF78B9E86CE0267588850B" />`

Now you will be able to decrypt your Bearer tokens.

Doesn't work for selfhosted apps!


using System;
using Pulumi;
using Pulumi.Aws.Kms;
using KmsAlias = Pulumi.Aws.Kms.Alias;
using System.Collections.Generic;

return await Deployment.RunAsync(() =>
{
    var key = new Key("shush-rs-kms-key", new KeyArgs()
    {
        DeletionWindowInDays = 7,
        KeyUsage = "ENCRYPT_DECRYPT",
        CustomerMasterKeySpec = "SYMMETRIC_DEFAULT",
        Description = "Used for shush-rs Integration Testing",
    });

    var alias = new KmsAlias("shush-rs-kms-key-alias", new AliasArgs()
    {
        NamePrefix = "alias/shush-rs",
        TargetKeyId = key.Id
    });

    return new Dictionary<string, object?>
    {
        ["KeyId"] = key.Id,
        ["KeyAlias"] = alias.Name.Apply(name =>
        {
            const string prefix = "alias/";
            var index = name.IndexOf(prefix, StringComparison.Ordinal);
            return (index < 0) ? name : name.Remove(index, prefix.Length);
        })
    };
});
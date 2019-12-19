# GPMDP Release Finder

> Apologies for the salt. It's late and Azure angers me.

A hastily slapped-together answer to a problem we arguably shouldn't have.

GPMDP hasn't had a published release in 18 months and the bugs are mounting up. CI builds are apparently the only answer we're getting, but CircleCI doesn't show artifacts for some builds for some insane reason. So here we are.

GPMDP Release Finder will query the API of CircleCI or AppVeyor (depending on your chosen platform) and find you the latest successful build from `master` and link you to the artifacts for that build.

> AppVeyor doesn't allow querying build job details for public projects so we can't link those directly.

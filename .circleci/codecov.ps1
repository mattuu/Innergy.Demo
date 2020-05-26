param
(
  $token,
  $fName
)

$ver = (gci "$env:userprofile\.nuget\packages\codecov").Name
$cmd = "$env:userprofile\.nuget\packages\codecov\$ver\tools\codecov.exe";
$arg1 = "-f $fName";
$arg2 = "-t $token";
& $cmd $arg1 $arg2
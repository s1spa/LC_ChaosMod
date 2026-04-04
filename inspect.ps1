Add-Type -Path "D:\Games\steam1\steamapps\common\Lethal Company\Lethal Company_Data\Managed\Assembly-CSharp.dll"
$asm = [AppDomain]::CurrentDomain.GetAssemblies() | Where-Object { $_.GetName().Name -eq "Assembly-CSharp" }

Write-Host "=== MenuManager fields ==="
$mm = $asm.GetType("MenuManager")
$mm.GetFields(60) | ForEach-Object { $_.FieldType.Name + "  " + $_.Name }

Write-Host ""
Write-Host "=== IngamePlayerSettings fields ==="
$ips = $asm.GetType("IngamePlayerSettings")
$ips.GetFields(60) | ForEach-Object { $_.FieldType.Name + "  " + $_.Name }

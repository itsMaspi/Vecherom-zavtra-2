; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Vecherom-Zavtra 2"
#define MyAppVersion "0.9.3"
#define MyAppPublisher "Dasha"
#define MyAppURL "https://github.com/itsMaspi/Vecherom-zavtra-2"
#define MyAppExeName "Vecherom-zavtra 2.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{65B3F33C-8325-4D9D-9AC7-5921E4E72242}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
; Remove the following line to run in administrative install mode (install for all users.)
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=E:\UnityProjects\Vecherom-zavtra-2\Vecherom-zavtra 2\Build\bins
OutputBaseFilename=VZ2-Installer
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "catalan"; MessagesFile: "compiler:Languages\Catalan.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "E:\UnityProjects\Vecherom-zavtra-2\Vecherom-zavtra 2\Build\release\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\UnityProjects\Vecherom-zavtra-2\Vecherom-zavtra 2\Build\release\MonoBleedingEdge\*"; DestDir: "{app}\MonoBleedingEdge"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "E:\UnityProjects\Vecherom-zavtra-2\Vecherom-zavtra 2\Build\release\Vecherom-zavtra 2_Data\*"; DestDir: "{app}\Vecherom-zavtra 2_Data"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "E:\UnityProjects\Vecherom-zavtra-2\Vecherom-zavtra 2\Build\release\UnityCrashHandler64.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\UnityProjects\Vecherom-zavtra-2\Vecherom-zavtra 2\Build\release\UnityPlayer.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent


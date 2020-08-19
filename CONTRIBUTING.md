# ConfigurationChecker

This tool is meant to be used as a standalone support software to collect information about a machine with minimal requirements. The goal is to collect the maximum of details about the hardware, the software and to export it in a ZIP archive that can be then sent with an email to a support team. This tool can be customized to add more company-specific diagnostics for your products.

We only depend on the .NET Framework 3.5 in order to be compatible with older setups without any modification.
A basic GUI is also provided to explore the diagnostics and the results.

## How to use

By default, ConfigurationChecker runs in a console, and asks the current user where to save the final archive containing all the diagnostic information.

By using the following parameters, you can configure the default behavior
- `/fast` Runs all the diagnostics in "fast" mode. If they support "fast" mode, the diagnostics might skip long running collections to achieve fast results
- `/gui` Runs the basic GUI to browse the available diagnostics and their results

To provide an easy to use experience for the end users, you can rename the executable with "Gui" in the name, and the GUI will be enabled. If you rename the executable "SupportGui.exe", the GUI will open.

When built, different executables will be generated and stamped:
- `ConfigurationChecker.exe`, the default executable
- `ConfigurationCheckerGui.exe`, a stamped executable always running the GUI

## Provided diagnostics

- **Hardware**
    - **General**: Collect CPU and memory information. Tries to collect msinfo32 and dxdiag as well
    - **Discs**: List all available physical discs
    - **Serial**: List all available serial ports
- **Networking**
    - **Certificates**: List the root certificates installed on the machine
    - **IPConfiguration**: Collects the list of adapters and the routing table of the machine
    - **OpenPorts**: Check for locally open ports by connecting to external services. Useful to detect whitelisting firewall or other restrictive configurations
    - **ServiceConnection**: Check for DNS resolution, TCP connection and HTTP access to external services. Useful to detect whitelisting firewall or other restrictive configurations
    - **Misc**: Collects the hosts file
- **Products**
    - **Permissions**: Collects the permissions in specified directories, and detect default permission tampering
- **Software**
    - **Culture**: Collects local culture information
    - **Dependencies**: Detects installed .NET Framework, C++ Reditributable and Windows Installer versions
    - **Drives**: Lists local drives and mapped network drivers
    - **Environment**: Lists environment variables
    - **InstalledUpdates**: Lists installed Windows updates
    - **LastEvents**: Collects last Application events
    - **RunningProcesses**: List all running processes
    - **RunningServices**: List all running services
    - **Security**: Detects installed Antivirus, Firewall and Antispyware
    - **WindowsVersion**: Detects installed Windows version

## Development

ConfigurationChecker provides a basic diagnostic model allowing you to easily add new company-specific diagnostics.

When run, a **Diagnostic** collects a **DiagnosticResult** containing a **Report**, some **Items** (warnings and errors created from an analysis of the machine) and some **Artifacts** (files collected from the diagnostic).

In the model, a **Diagnostic** has a **RequiresElevation** property that is not supported yet. The tool will need to check for elevation requirements and runs as elevated process if needed.

{
  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixos-23.11";
    utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, utils }:
    utils.lib.simpleFlake {
      inherit self nixpkgs;
      name = "altibiz-mess";
      config = {
        allowUnfree = true;
      };
      overlay = (final: prev: {
        nodejs = prev.nodejs_20;
        dotnet-sdk = prev.dotnet-sdk_7;
      });
      shell = { pkgs }:
        pkgs.mkShell {
          packages = with pkgs; let
            mess =
              (pkgs.writeShellApplication {
                name = "mess";
                runtimeInputs = [ pkgs.nodePackages.yarn ];
                text = ''
                  echo 'require("husky")' |
                    yarn node >/dev/null 2>&1 ||
                    (yarn && yarn scripts start prepare --skip test)

                  yarn scripts start "$@"
                '';
              });

            usql = pkgs.writeShellApplication {
              name = "usql";
              runtimeInputs = [ pkgs.usql ];
              text = ''
                usql pg://mess:mess@localhost/mess?sslmode=disable "$@"
              '';
            };
          in
          [
            # Nix
            nil
            nixpkgs-fmt

            # C#
            dotnet-sdk
            dotnet-runtime
            dotnet-aspnetcore
            omnisharp-roslyn
            netcoredbg

            # Web
            nodejs
            nodePackages.yarn
            nodePackages.typescript-language-server
            nodePackages.vscode-langservers-extracted
            nodePackages.yaml-language-server

            # Misc
            usql
            just
            nodePackages.prettier
            nodePackages.yaml-language-server
            marksman
            taplo

            # Tools
            mess
          ];
        };
    };
}

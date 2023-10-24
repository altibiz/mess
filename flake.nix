{
  inputs = {
    # NOTE: Node 20.4.0
    nixpkgs.url = "github:nixos/nixpkgs?rev=fc4810bfca66fc4f3a8f5d4cceb1621e79bc91bb";
    utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, utils }:
    utils.lib.simpleFlake {
      inherit self nixpkgs;
      name = "mess";
      config = {
        allowUnfree = true;
      };
      overlay = (final: prev: {
        nodejs = prev.nodejs_20;
        dotnet-sdk = prev.dotnet-sdk_7;
      });
      shell = { pkgs }:
        pkgs.mkShell {
          packages = with pkgs; [
            (pkgs.writeShellApplication {
              name = "mess";
              runtimeInputs = [ pkgs.nodePackages.yarn ];
              text = ''
                echo 'require("prettier")' |
                  yarn node >/dev/null 2>&1 ||
                  (printf "%s %s %s" \
                    "\`prettier\` not found." \
                    "Please make sure you run \`mess prepare\`" \
                    "before running any other commands" && yarn)

                yarn scripts start "$@"
              '';
            })
            # TODO: https://github.com/dotnet/sdk/issues/30546
            (pkgs.buildDotnetGlobalTool {
              pname = "dotnet-csharpier";
              nugetName = "CSharpier";
              version = "0.25.0";
              nugetSha256 = "sha256-7yRDI7vdLTXv0XuUHKUdsIJsqzmw3cidWjmbZ5g5Vvg=";
              dotnet-sdk = pkgs.dotnetCorePackages.sdk_6_0;
              dotnet-runtime = pkgs.dotnetCorePackages.sdk_6_0;
              meta = with pkgs.lib; {
                homepage = "https://github.com/belav/csharpier";
                changelog = "https://github.com/belav/csharpier/blob/main/CHANGELOG.md";
                license = licenses.mit;
                platforms = platforms.linux;
              };
            })
            nil
            nixpkgs-fmt
            direnv
            nix-direnv
            git
            helix
            lazygit
            nodejs
            bun
            nodePackages.yarn
            nodePackages.typescript-language-server
            nodePackages.vscode-langservers-extracted
            nodePackages.yaml-language-server
            dotnet-sdk
            omnisharp-roslyn
            netcoredbg
            docker-client
            docker-compose
          ];
        };
    };
}

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
            git
            helix
            lazygit
            nodejs_20
            bun
            nodePackages.yarn
            nodePackages.typescript-language-server
            nodePackages.vscode-langservers-extracted
            nodePackages.yaml-language-server
            dotnet-sdk_7
            omnisharp-roslyn
            netcoredbg
            docker-client
            docker-compose
          ];
        };
    };
}

{
  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";
    utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, utils }:
    utils.lib.simpleFlake {
      inherit self nixpkgs;
      # FIX: error: 'mess' has been renamed to/replaced by 'mame'
      name = "mess-flake";
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
              runtimeInputs = [ pkgs.nodejs pkgs.bun ];
              text = ''
                echo 'require("prettier")' |
                  node >/dev/null 2>&1 ||
                  (printf "%s %s %s" \
                    "\`prettier\` not found." \
                    "Please make sure you run \`mess prepare\`" \
                    "before running any other commands" && bun install)

                bun run scripts start "$@"
              '';
            })
            git
            lazygit
            helix
            nil
            nixpkgs-fmt
            nodejs
            bun
            dotnet-sdk
            omnisharp-roslyn
            netcoredbg
            powershell
            docker-client
            docker-compose
            lazydocker
          ];
        };
    };
}

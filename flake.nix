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
      overlay = (final: prev: {
        nodejs = prev.nodejs_20;
      });
      shell = { pkgs }:
        pkgs.mkShell {
          packages = with pkgs; [
            nodejs_20
            bun
            nodePackages.yarn
            nodePackages.typescript-language-server
            nodePackages.yaml-language-server
            dotnet-sdk_7
            omnisharp-roslyn
          ];
        }
      ;
    };
}

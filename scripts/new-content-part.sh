#!/usr/bin/env bash
set -eo pipefail

abs() { echo "$(cd "$(dirname "$1")" && pwd)/$(basename "$1")"; }
SCRIPT_DIR="$(dirname "$(abs "$0")")"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"

MODULE="$1"
if [ ! "$MODULE" ]; then
  printf "[Mess] First argument (module) must not be empty\n"
  exit 1
fi

NAME="$2"
if [ ! "$NAME" ]; then
  printf "[Mess] Second argument (name) must not be empty\n"
  exit 1
fi

ABSTRACTIONS_NAMESPACE="Mess.$MODULE.Abstractions"
ABSTRACTIONS_BASE_DIR="$ROOT_DIR/src/Mess.Abstractions/$ABSTRACTIONS_NAMESPACE"
MODELS="$ABSTRACTIONS_BASE_DIR/Models"
PART="$MODELS/${NAME}Part.cs"

NAMESPACE="Mess.$MODULE"
BASE_DIR="$ROOT_DIR/src/Mess.Modules/$NAMESPACE"
VIEW_MODELS="$BASE_DIR/ViewMdoels"
VIEW_MODEL="$VIEW_MODELS/${NAME}PartViewModel.cs"
VIEWS="$BASE_DIR/Views"
DISPLAY_OPTIONS_VIEW="$VIEWS/${NAME}Part.DisplayOptions.cshtml"
EDITOR_OPTIONS_VIEW="$VIEWS/${NAME}Part.Option.cshtml"
VIEW="$VIEWS/${NAME}Part.cshtml"
EDIT_VIEW="$VIEWS/${NAME}Part.Edit.cshtml"
SUMMARY_VIEW="$VIEWS/${NAME}Part.Summary.cshtml"
DRIVERS="$BASE_DIR/Drivers"
DISPLAY_DRIVER="$DRIVERS/${NAME}PartDisplayDriver.cs"

if [ ! -d "$MODELS" ]; then
  mkdir -p "$MODELS"
fi

if [ ! -d "$VIEW_MODELS" ]; then
  mkdir -p "$VIEW_MODELS"
fi

if [ ! -d "$VIEWS" ]; then
  mkdir -p "$VIEWS"
fi

if [ ! -d "$DRIVERS" ]; then
  mkdir -p "$DRIVERS"
fi

cat <<END >"$PART"
using OrchardCore.ContentManagement;

namespace ${ABSTRACTIONS_NAMESPACE}.Models;

public class ${NAME}Part : ContentPart
{
}
END

cat <<END >"$VIEW_MODEL"
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement.Metadata.Models;
using ${ABSTRACTIONS_NAMESPACE}.Models;

namespace ${NAMESPACE}.ViewModels;

public class ${NAME}ViewModel
{
  [BindNever]
  public ${NAME}Part Part { get; set; } = default!;

  [BindNever]
  public ContentTypePartDefinition Definition { get; set; } = default!;
}
END

cat <<END >"$DISPLAY_OPTIONS_VIEW"
@{
  string currentDisplayMode = Model.DisplayMode;
}
<option value="" selected="@String.IsNullOrEmpty(currentDisplayMode)">@T["Standard"]</option>
END

cat <<END >"$EDITOR_OPTIONS_VIEW"
@{
    string currentEditor = Model.Editor;
}
<option value="" selected="@String.IsNullOrEmpty(currentEditor)">@T["Standard"]</option>
END

cat <<END >"$VIEW"
@model ${NAME}PartViewModel
END

cat <<END >"$EDIT_VIEW"
@model ${NAME}PartViewModel
END

cat <<END >"$SUMMARY_VIEW"
@model ${NAME}PartViewModel
END

cat <<END >"$DISPLAY_DRIVER"
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;
using ${ABSTRACTIONS_NAMESPACE}.Models;
using ${NAMESPACE}.ViewModels;

namespace ${NAMESPACE}.Drivers;

public class ${NAME}PartDisplayDriver : ContentPartDisplayDriver<${NAME}Part>
{
  public override IDisplayResult Display(
    ${NAME}Part part,
    BuildPartDisplayContext context
  )
  {
    return Initialize<${NAME}PartViewModel>(
      GetDisplayShapeType(context),
      model =>
      {
        model.Part = part;
        model.Definition = context.TypePartDefinition;
      }
    );
  }

  public override IDisplayResult Edit(
    ${NAME}Part part,
    BuildPartEditorContext context
  )
  {
    return Initialize<${NAME}PartViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.Part = part;
        model.Definition = context.TypePartDefinition;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    ${NAME}Part part,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var viewModel = new ${NAME}PartViewModel();

    if (await updater.TryUpdateModelAsync(viewModel, Prefix))
    {
      return Edit(part, context);
    }

    return Edit(part, context);
  }

  public ${NAME}PartDisplayDriver(
    IStringLocalizer<${NAME}PartDisplayDriver> localizer
  )
  {
    S = localizer;
  }

  private readonly IStringLocalizer S;
}
END

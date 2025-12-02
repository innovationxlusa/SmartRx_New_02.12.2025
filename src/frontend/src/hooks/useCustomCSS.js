import { useTheme } from "@mui/material/styles";
import { getCSSVariableValue } from "../utils/utils";
import useMediaQuery from "@mui/material/useMediaQuery";

export const useCustomCSS = () => {
    const theme = useTheme();

    const PRIMARY_COLOR = getCSSVariableValue("--primary");
    const WHITE_COLOR = getCSSVariableValue("--white");
    const BLACK_COLOR = getCSSVariableValue("--black");

    const IS_MOBILE = useMediaQuery(theme.breakpoints.down("sm"));

    const TEXT_COLOR = theme.palette.mode === "dark" ? "text-white" : "text-black";
    const BORDER = theme.palette.mode === "dark" ? "border-light" : "border-dark";
    const MODAL_BORDER = theme.palette.mode === "dark" ? "border-secondary border-1" : "border-dark border-3";
    const BORDER_RIGHT = theme.palette.mode === "dark" ? "2px solid white" : "2px solid black";
    const BORDER_COLOR = theme.palette.action.focus;
    const PLACEHOLDER_CLASS = theme.palette.mode === "dark" ? "custom-placeholder-light" : "custom-placeholder-dark";
    const TABLE_BACKGROUND_COLOR = theme.palette.mode === "dark" ? "table-dark" : "";
    const DEFAULT_BACKGROUND_COLOR = theme.palette.background.default;
    const DEFAULT_FONT_COLOR = theme.palette.primary.fontColor;
    const ACTIVE_FONT_COLOR = theme.palette.primary.main;
    const PRIMARY_FONT_COLOR = theme.palette.text.primary;
    const BACKGROUND_COLOR = {
        background: theme.palette.mode === "dark" ? BLACK_COLOR : WHITE_COLOR,
    };
    const BACKGROUND_COLOR_STYLE = {
        backgroundColor: theme.palette.mode === "dark" ? "rgba(255, 255, 255, 0.13) !important" : "rgb(255, 255, 255) !important",
        border: theme.palette.mode === "dark" ? "1px solid #4d5154 !important" : "1px solid #dee2e6 !important",
    };

    return {
        PRIMARY_COLOR,
        WHITE_COLOR,
        BLACK_COLOR,
        IS_MOBILE,
        TEXT_COLOR,
        BORDER,
        MODAL_BORDER,
        BORDER_RIGHT,
        BORDER_COLOR,
        PLACEHOLDER_CLASS,
        TABLE_BACKGROUND_COLOR,
        DEFAULT_BACKGROUND_COLOR,
        DEFAULT_FONT_COLOR,
        ACTIVE_FONT_COLOR,
        BACKGROUND_COLOR,
        PRIMARY_FONT_COLOR,
        BACKGROUND_COLOR_STYLE,
    };
};

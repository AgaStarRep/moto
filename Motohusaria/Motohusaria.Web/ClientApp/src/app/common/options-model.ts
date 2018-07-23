export interface OptionsModel{
    results: OptionItemModel[],
    pagination: OptionPaginationModel

}

export interface OptionItemModel{
    text: string,
    id: string,
}

export interface OptionPaginationModel{
    more: boolean
}